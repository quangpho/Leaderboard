import { config } from "./config";

export async function getLeaderBoard(playerId: number) {
    const url = `${config.apiUrl}/leaderboard/${playerId}`;
    console.log("Fetching from URL:", url);
    const res = await fetch(url, {
        method: "GET",
    });

    console.log("API Response:", res);
    if (!res.ok) throw new Error("API error");
    return res.json();
}