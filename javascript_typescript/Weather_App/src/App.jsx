import { useEffect, useState } from "react"
import { getDailyForecastByCity, getWeatherByCity } from "./api/weatherAPI";
import { CityWeatherCard } from "./components/CityWeatherCard"

function App() {
  const [city, setCity] = useState("Toronto");
  const [inputCity, setInputCity] = useState('Toronto');
  const [weatherData, setWeatherData] = useState(null);
  const [forecastList, setForecastList] = useState([])
  const [error, setError] = useState("");

  useEffect(() => {
    const getWeather = async () => {
      try{
        const responseWeatherData = await getWeatherByCity(city);
        setWeatherData(responseWeatherData.data);
        
        const { lat, lon } = responseWeatherData.data.coord;

        const responseForecastList = await getDailyForecastByCity(lat, lon);

        setForecastList(responseForecastList.data.list.slice(0, 4));

        setError("")

      } catch (e) {
        setError("City Not Found");
        console.error("Error: ", e)
        setWeatherData(null);
        setForecastList([])
      }
    }

    getWeather()
  }, [city]);

  const handleSubmit = (e) => {
    e.preventDefault();
    setCity(inputCity);
  };

  return (
    <div className="container py-4">
      <h1 className="text-center mb-4">Weather App</h1>

      <form onSubmit={handleSubmit} className="d-flex justify-content-center mb-3 input-group">
        <input
          type="text"
          className="form-control"
          value={inputCity}
          onChange={(e) => setInputCity(e.target.value)}
          placeholder="Enter city..."
        />
        <button type="submit" className="btn btn-primary">Search</button>
      </form>

      {error && <p className="text-danger text-center fw-semibold">{error}</p>}

      <CityWeatherCard weatherData={weatherData} forecastList={forecastList}/>
    </div>
  )
}

export default App
