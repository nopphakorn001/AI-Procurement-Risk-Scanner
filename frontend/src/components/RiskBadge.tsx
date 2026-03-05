interface Props {
  score: number | null;
}

export function RiskBadge({ score }: Props) {
  if (score === null) {
    return <span style={styles.pending}>Pending</span>;
  }

  const color =
    score < 33 ? "#16a34a"
    : score < 66 ? "#ca8a04"
    : "#dc2626";

  return (
    <span style={{ ...styles.badge, backgroundColor: color }}>
      {score.toFixed(1)}
    </span>
  );
}

const styles = {
  badge: {
    color: "#fff",
    padding: "2px 10px",
    borderRadius: "12px",
    fontWeight: 600,
    fontSize: "0.85rem",
  } as React.CSSProperties,
  pending: {
    color: "#6b7280",
    fontStyle: "italic",
    fontSize: "0.85rem",
  } as React.CSSProperties,
};
