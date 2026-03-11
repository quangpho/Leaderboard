"use client";

import { useState } from "react";
import {
  submitScore,
} from "../../features/leaderboard.action";

export default function SubmitPage() {

  return (
    <div style={{ maxWidth: "400px", margin: "50px auto", padding: "20px" }}>
      <h1>Submit Your Score</h1>
      <form action={submitScore}>
        <label htmlFor="playerId">Player ID:</label>
        <input
          type="number"
          placeholder="Enter your player ID"
          name = "playerId"
          style={{ width: "100%", padding: "8px", marginBottom: "10px" }}
        />
        <label htmlFor="score">Score:</label>
        <input
          type="number"
          placeholder="Enter your score"
          name = "score"
          style={{ width: "100%", padding: "8px", marginBottom: "10px" }}
        />
        <button type="submit" style={{ width: "100%", padding: "10px" }}>
          Submit Score"
        </button>
      </form>
      {/* {message && <p style={{ marginTop: "10px" }}>{message}</p>} */}
    </div>
  );
}
