export function ForecastItem ({ date, iconURL, temp }) {
    const dateMilliseconds = new Date(date * 1000);

    return (
        <div className="text-center p-3 bg-light border rounded-3 shadow-sm forecast-item me-3">
            <p className="text-xs text-muted mb-1">
                {dateMilliseconds.toLocaleDateString(undefined, { weekday: "short" })}
            </p>
            
            <p className="text-xs text-muted mb-1">
                {dateMilliseconds.toLocaleTimeString(undefined, { hour: "2-digit", minute: "2-digit" })}
            </p>
            
            <img src={iconURL} alt="Weather Icon" className="w-100 h-100 mx-auto" />
            
            <p className="fw-bold text-dark mt-1">{temp}°C</p>
        </div>
    );
};