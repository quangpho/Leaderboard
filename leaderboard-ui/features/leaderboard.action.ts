"use server";

import { revalidatePath } from "next/cache";
import { config } from "../lib/config";

export interface SubmitScoreResponse {
  score: number;
  rank: number;
  status: string;
}

export async function submitScore(
  formData: FormData,
): {
  const playerId = Number(formData.get("playerId"));
  const score = Number(formData.get("score"));
  const url = `${config.baseUrl}/leaderboard/submit`;
  const response = await fetch(url, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ playerId, score }),
    cache: 'no-store',
  });
  revalidatePath("/leaderboard");
}
