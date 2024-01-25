﻿using Microsoft.AspNetCore.Components.Forms;
using SSW.Rewards.Shared.DTOs.Quizzes;

namespace SSW.Rewards.Application.Quizzes.Commands.AddNewQuiz;
public class AdminUpdateQuiz : IRequest<int>
{
    public QuizEditDto Quiz { get; set; } = new();
}

public class AdminUpdateQuizHandler : IRequestHandler<AdminUpdateQuiz, int>
{
    private readonly IQuizImageStorageProvider _storage;
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserService _userService;

    public AdminUpdateQuizHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IUserService userService,
        IQuizImageStorageProvider storage)
    {
        _context = context;
        _currentUserService = currentUserService;
        _userService = userService;
        _storage = storage;
    }

    public async Task<int> Handle(AdminUpdateQuiz request, CancellationToken cancellationToken)
    {

        var dbQuiz = await _context.Quizzes
                                    .Include(q => q.Questions)
                                    .ThenInclude(r => r.Answers)
                                    .FirstAsync(x => x.Id == request.Quiz.QuizId, cancellationToken);
        
        if (request.Quiz.IsCarousel && request.Quiz.CarouselImageFile != null)
        {
            dbQuiz.CarouselPhoto = await UploadQuizImage(request.Quiz.CarouselImageFile, cancellationToken);
        }
        
        
        if (request.Quiz.ThumbnailImageFile != null)
        {
            dbQuiz.ThumbnailPhoto = await UploadQuizImage(request.Quiz.ThumbnailImageFile, cancellationToken);
        }
        
        dbQuiz.Title = request.Quiz.Title;
        dbQuiz.Description = request.Quiz.Description;
        dbQuiz.LastUpdatedUtc = DateTime.UtcNow;
        dbQuiz.IsArchived = request.Quiz.IsArchived;
        dbQuiz.Icon = request.Quiz.Icon;
        dbQuiz.IsCarousel = request.Quiz.IsCarousel;

        // loop through the incoming quiz's questions and add/update/delete them from the dbquiz
        foreach (var q in request.Quiz.Questions)
        {
            if (q.QuestionId > 0)
            {
                var existingQuestion = dbQuiz.Questions.First(x => x.Id == q.QuestionId);
                UpdateExistingQuestion(ref existingQuestion, q);
            }
            else
            {
                // New question. Add it.
                dbQuiz.Questions.Add(CreateQuestion(q));
            }
        }
        _context.Quizzes.Update(dbQuiz);
        await _context.SaveChangesAsync(cancellationToken);
        return dbQuiz.Id;
    }

    private void UpdateExistingQuestion(ref QuizQuestion existingQuestion, QuizQuestionEditDto dto)
    {
        existingQuestion.Text = dto.Text;

        // get a list of all answerIds that exist in the dbrecord,
        // so once we finish the loop beneath, we can see what answer Ids remain
        // in the array. Any remaining Ids are answers that should be deleted.
        List<int> currentAnswerIds = existingQuestion.Answers.Select(r => r.Id).ToList();

        // loop through the answers to add/update/delete them from the existingQuestion
        foreach (var a in dto.Answers)
        {
            if (a.QuestionAnswerId > 0)
            {
                // existing answer. Update it
                currentAnswerIds.Remove(a.QuestionAnswerId);

                var existingAnswer = existingQuestion.Answers.First(x => x.Id == a.QuestionAnswerId);
                existingAnswer.Text = a.Text;
                existingAnswer.IsCorrect = a.IsCorrect;
            }
            else
            {
                // new answer. Add it.
                QuizAnswer answer = new QuizAnswer
                {
                    QuestionId = existingQuestion.Id,
                    Text = a.Text,
                    CreatedUtc = DateTime.UtcNow,
                    IsCorrect = a.IsCorrect
                };
                existingQuestion.Answers.Add(answer);
            }
        }
        foreach (int i in currentAnswerIds)
        {
            QuizAnswer answerToBeDeleted = existingQuestion.Answers.First(x => x.Id == i);
            existingQuestion.Answers.Remove(answerToBeDeleted);
        }
    }
    private QuizQuestion CreateQuestion(QuizQuestionEditDto dto)
    {
        var dbQuestion = new QuizQuestion
        {
            Text = dto.Text,
            CreatedUtc = DateTime.UtcNow,
            Answers = dto.Answers.Select(a => new QuizAnswer
            {
                IsCorrect = a.IsCorrect,
                Text = a.Text
            }).ToList()
        };
        return dbQuestion;
    }

    private async Task<string> UploadQuizImage(IBrowserFile file, CancellationToken cancellationToken)
    {
        var stream = file.OpenReadStream(file.Size);
        
        await using var ms = new MemoryStream();
        await stream.CopyToAsync(ms, cancellationToken);

        byte[] bytes = ms.ToArray();

        string filename = Guid.NewGuid().ToString();

        return await _storage.UploadCarouselImage(bytes, filename);
    }
}
