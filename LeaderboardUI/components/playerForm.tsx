'use client';

import { useState } from 'react';
import { getLeaderBoard } from '../lib/api';
import Table from './table';

type Response = {
    topScores: { playerId: number; score: number, rank: number, lastSubmitDate: string }[],
    nearbyScores: { playerId: number; score: number, rank: number, lastSubmitDate: string }[];
};

export function PlayerForm() {
    const [inputValue, setInputValue] = useState('');
    const [data, setData] = useState<Response>();
    const handleClick = async () => {

        const response = await getLeaderBoard(Number(inputValue));
        console.log("Leaderboard Data:", response);
        setData(response);
    };

    return (
        <div>
            <input
                type="number"
                value={inputValue}
                onChange={(e) => setInputValue(e.target.value)}
                onKeyDown={(e) => e.key === 'Enter' && handleClick()}
                placeholder="Type something..."
            />
            <button onClick={handleClick}>
                Submit
            </button>

            {
                data ? (
                    <div>
                        <Table data={data.topScores} name='Top Scores' />
                        <Table data={data.nearbyScores} name='Relevant Scores' />
                    </div>

                ) : (
                    <p>No data found for player ID {inputValue}</p>
                )
            }
        </div>

    )
}