import { useMutation } from "@tanstack/react-query";
import { loginUser, signupUser } from "../services/authService";
import { useDispatch } from "react-redux";
import { loginSuccess } from "../redux/slices/authSlice";
import { useNavigate } from "react-router-dom";

export const useLogin = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();

    return useMutation({
        mutationFn: (credentials) => loginUser(credentials),
        onSuccess: (data) => {
            dispatch(loginSuccess(data.token));
            navigate("/dashboard");
        }
    });
};

export const useSignup = () => {
    return useMutation({
        mutationFn: (userData) => signupUser(userData),
    });
};
