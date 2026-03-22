import axios from "axios";
import { useForm } from "react-hook-form";
import { useNavigate, Link } from "react-router-dom";

export default function Login(){
    const navigate = useNavigate();

    const {register, handleSubmit} = useForm();

    const onSubmit = async (data) => {
        try{
            const res = await axios.post("http://localhost:3000/api/login", data);
            localStorage.setItem("token", res.data.token);
            localStorage.setItem("username", res.data.username)
            navigate("/chat")
        } catch (e){
            alert("Login failed")
        }
    }

    return (
        <div className="h-screen flex items-center justify-center bg-gray-50">
            <div className="w-80 bg-white p-6 border border-gray-300 rounded-lg">
                <h2 className="text-xl font-semibold mb-6 text-gray-800">Login</h2>

                <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-4">
                <input
                    {...register("username", { required: true })}
                    placeholder="Username"
                    className="p-2 border border-gray-300 rounded focus:outline-none text-sm"
                />
                <input
                    type="password"
                    {...register("password", { required: true })}
                    placeholder="Password"
                    className="p-2 border border-gray-300 rounded focus:outline-none text-sm"
                />
                <button className="bg-green-600 text-white py-2 rounded text-sm">
                    Login
                </button>
                </form>

                <Link to="/signup" className="block mt-4 text-center text-xs text-gray-500">
                    Sign Up
                </Link>
            </div>
        </div>
    )
}