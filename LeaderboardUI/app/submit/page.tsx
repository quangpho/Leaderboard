"use client";

import { useState } from "react";
import {
  submitScore,
  SubmitScoreResponse,
} from "../../features/leaderboard.api";

export default function SubmitPage() {
  const [score, setScore] = useState<number>(0);
  const [playerId, setPlayerId] = useState<number>(0);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!score) {
      setMessage("Please enter a score");
      return;
    }

    if (!playerId) {
      setMessage("Please enter a player ID");
      return;
    }

    const data: SubmitScoreResponse = await submitScore(playerId, score);
  };

  return (
    <div style={{ maxWidth: "400px", margin: "50px auto", padding: "20px" }}>
      <h1>Submit Your Score</h1>
      <form onSubmit={handleSubmit}>
        <label htmlFor="playerId">Player ID:</label>
        <input
          type="number"
          value={playerId}
          onChange={(e) => setPlayerId(Number(e.target.value))}
          placeholder="Enter your player ID"
          disabled={loading}
          style={{ width: "100%", padding: "8px", marginBottom: "10px" }}
        />
        <label htmlFor="score">Score:</label>
        <input
          type="number"
          value={score}
          onChange={(e) => setScore(Number(e.target.value))}
          placeholder="Enter your score"
          disabled={loading}
          style={{ width: "100%", padding: "8px", marginBottom: "10px" }}
        />
        <button
          type="submit"
          disabled={loading}
          style={{ width: "100%", padding: "10px" }}
        >
          {loading ? "Submitting..." : "Submit Score"}
        </button>
      </form>
      {message && <p style={{ marginTop: "10px" }}>{message}</p>}
    </div>
  );
}
