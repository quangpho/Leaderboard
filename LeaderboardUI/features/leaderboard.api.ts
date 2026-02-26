import { get } from '../lib/api-client';
import { config } from '../lib/config';
import { Player } from '../types/player';

export interface LeaderBoardResponse {
    topScores: Player[];
    nearbyScores: Player[];
}

export async function findById(playerId: number): Promise<LeaderBoardResponse> {
    const url = `${config.baseUrl}/leaderboard/${playerId}`;
    const data: LeaderBoardResponse = await get(url);

    console.log("API Response:", data);
    return data;
}