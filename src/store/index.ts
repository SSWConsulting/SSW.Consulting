import {
  LeaderboardUserDto,
  UserClient,
  AchievementClient,
  LeaderboardClient,
  RewardClient,
  StaffClient,
  BaseClient,
  AchievementViewModel
} from "../services";
import { DecodedJWT } from "../models";
import { RewardAdminViewModel } from '../services/SSW-Rewards-client';

export interface State {
  users?: LeaderboardUserDto[];
  achievements: AchievementViewModel[];
  rewards: RewardAdminViewModel[];
  authenticated: boolean;
  authorised: boolean;
  currentUser?: DecodedJWT;
  token?: string;
  leaderboardClient: LeaderboardClient;
  rewardClient: RewardClient;
  staffClient: StaffClient;
  userClient: UserClient;
  achievementClient: AchievementClient;
}

// @ts-ignore
const API_URL = window.config.apiUrl as string;


export const createInitialState = (baseUrl: string = API_URL) => {
  return {
    leaderboardClient: new LeaderboardClient(baseUrl),
    rewardClient: new RewardClient(baseUrl),
    staffClient: new StaffClient(baseUrl),
    userClient: new UserClient(baseUrl),
    achievementClient: new AchievementClient(baseUrl),
    authenticated: false,
    authorised: false
  } as State;
};

type SSWRewardsHttpClient =
  | LeaderboardClient
  | RewardClient
  | StaffClient
  | UserClient
  | AchievementClient;

export function setAuth<T extends SSWRewardsHttpClient>(
  client: T,
  token: string
) {
  if (client.token) {
    return token;
  }
  client.setAuthToken(token ? token : "");
  return client;
}
