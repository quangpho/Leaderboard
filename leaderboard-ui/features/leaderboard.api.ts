import { get } from '../lib/api-client';
import { config } from '../lib/config';
import { Player } from '../types/player';

export interface LeaderBoardResponse {
    topScores: Player[];
    nearbyScores: Player[];
}

export interface SubmitScoreResponse {
    score: number;
    rank: number;
    status: string;
}

export async function findById(playerId: number): Promise<LeaderBoardResponse> {
    const url = `${config.baseUrl}/leaderboard/${playerId}`;
    const data: LeaderBoardResponse = await get(url);

    console.log("API Response:", data);
    return data;
}

export async function submitScore(playerId: number, score: number): Promise<SubmitScoreResponse> {
    const url = `${config.baseUrl}/leaderboard/submit`;
    const response = await fetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ playerId, score }),
    });
    const data: SubmitScoreResponse = await response.json();
    return data;
}

