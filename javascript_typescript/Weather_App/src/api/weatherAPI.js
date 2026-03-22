import axios from "axios";

const axiosClient = axios.create({
    baseURL: "https://api.openweathermap.org/data/2.5",
    headers: { "Content-Type": "application/json" }
})

const API_KEY = "";

export const getWeatherByCity = async (city) => {
    return axiosClient.get(`/weather?q=${city}&appid=${API_KEY}`);
};

export const getDailyForecastByCity = async (lat, lon) => {
    return axiosClient.get(`/forecast?lat=${lat}&lon=${lon}&units=metric&appid=${API_KEY}`);
};