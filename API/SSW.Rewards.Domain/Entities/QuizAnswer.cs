﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SSW.Rewards.Domain.Entities
{
    public class QuizAnswer : Entity
    {
        public int QuestionId { get; set; }
        public virtual QuizQuestion Question { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
