import { Player } from "../types/player";

interface TableProps {
  data: Player[];
  name: string;
}

export default function Table({ data, name }: TableProps) {
  return (
    <div>
      <h2>{name}</h2>
      <table border={1} cellPadding={8}>
        <thead>
          <tr>
            <th>ID</th>
            <th>Score</th>
            <th>Rank</th>
            <th>Last Submit</th>
          </tr>
        </thead>
        <tbody>
          {data.map((item) => (
            <tr key={item.playerId}>
              <td>{item.playerId}</td>
              <td>{item.score}</td>
              <td>{item.rank}</td>
              <td>{item.lastSubmitDate}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
