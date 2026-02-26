"use client";

import { useState } from "react";
import { findById, LeaderBoardResponse } from "../features/leaderboard.api";
import Table from "./Table";

export function PlayerForm() {
  const [inputPlayerId, setInputPlayerId] = useState(0);
  const [data, setData] = useState<LeaderBoardResponse | null>(null);
  const handleClick = async () => {
    const response = await findById(Number(inputPlayerId));
    console.log("Leaderboard Data:", response);
    setData(response);
  };

  return (
    <div>
      <input
        type="number"
        value={inputPlayerId}
        onChange={(e) => setInputPlayerId(Number(e.target.value))}
        onKeyDown={(e) => e.key === "Enter" && handleClick()}
      />
      <button onClick={handleClick}>Submit</button>

      {data ? (
        <div>
          <Table data={data.topScores} name="Top Scores" />
          <Table data={data.nearbyScores} name="Relevant Scores" />
        </div>
      ) : (
        <p>No data found for player ID {inputPlayerId}</p>
      )}
    </div>
  );
}
