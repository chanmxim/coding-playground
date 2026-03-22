import api from "../api/axiosClient";

export const loginUser = async (credentials) => {
    const response = await api.post("/user/login", credentials);
    return response.data;
};

export const signupUser = async (userData) => {
    const response = await api.post("/user/signup", userData);
    return response.data;
};
