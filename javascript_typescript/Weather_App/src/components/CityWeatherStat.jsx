export function CityWeatherStat ({ label, value, unit }){

    return (
        <div className="d-flex justify-content-between align-items-center py-2 px-3 stat-item">
            <p className="text-sm text-muted">{label}</p>
            <p className="fs-6 fw-bold text-dark">{value} {unit}</p>
        </div>
    )
};