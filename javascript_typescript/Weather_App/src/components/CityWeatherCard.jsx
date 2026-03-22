import { CityWeatherStat } from "./CityWeatherStat"
import { ForecastItem } from "./ForecastItem";

export function CityWeatherCard ({weatherData, forecastList}){
    if (!weatherData) return null;

    const { name, main, weather, wind, visibility, clouds, sys } = weatherData; 

    return (
        <div className="card shadow-lg rounded-4 weather-card mt-5 mx-auto" id="wrapper">
            <div className="card-body p-4 p-lg-5 row g-4">
                <div className="col-12 col-lg-4 d-flex flex-column align-items-center justify-content-center p-4 main-weather-display rounded-3 shadow-sm">
                    <div className="blue-background">
                        <h3 className="fs-2 fw-bold text-black mb-1">{name}, {sys.country}</h3>

                        <div className="s-5 text-muted mb-4 text-capitalize d-flex justify-content-center align-items-center">
                            <h4 className="fs-1 fw-bolder text-black">{Math.round(main.temp - 273.15)}&deg;C</h4>
                        </div>  
                    </div>

                </div>

                <div className="col-12 col-lg-8">
                    <div className="row g-4">

                        {/* Weather Statistics */}
                        <div className="col-12 col-md-6">
                            <div className="card h-100 shadow-sm border-0">
                                <div className="card-body p-3 ">
                                    <h4 className="fs-5 fw-semibold text-secondary mb-3 pb-2 border-bottom">Current Conditions</h4>
                                    <div className="d-flex flex-column">
                                        <CityWeatherStat key={0} label="Humidity" value={main.humidity} unit="%"/>
                                        <CityWeatherStat key={1} label="Pressure" value={main.pressure} unit="hPa"/>
                                        <CityWeatherStat key={2} label="Wind" value={wind.speed} unit="m/s"/>
                                        <CityWeatherStat key={3} label="Visibility" value={visibility} unit="%"/>
                                    </div>
                                </div>
                            </div>
                        </div>

                        {/* Weather Forecast */}
                        <div className="col-12 col-md-6">
                            <div className="card h-100 shadow-sm border-0">
                                <div className="card-body p-3 d-flex flex-column">
                                    <h4 className="fs-5 fw-semibold text-secondary mb-3 pb-2 border-bottom">
                                        5-Day Forecast
                                    </h4>
                                    <div >
                                        {forecastList?.map((forecastItem, index) => (
                                        < ForecastItem 
                                            key={index}
                                            date={forecastItem.dt} 
                                            iconURL={`http://openweathermap.org/img/wn/${forecastItem.weather[0].icon}@2x.png` } 
                                            temp={forecastItem.main.temp}/>
                                        ))}
                                    </div>
                                </div>
                            </div>
                        </div>
                
                    </div>
                </div>
                
            </div>
        </div>
    )
}