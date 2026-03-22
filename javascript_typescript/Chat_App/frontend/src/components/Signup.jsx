import axios from "axios";
import { useForm } from "react-hook-form";
import { useNavigate, Link } from "react-router-dom";

export default function Signup(){
    const navigate = useNavigate();

    const {register, handleSubmit} = useForm();

    const onSubmit = async (data) => {
        try{
            await axios.post("http://localhost:3000/api/signup", data);
            navigate("/login")
        } catch (e){
            alert("Signup failed")
        }
    }

    return (
        <div className="h-screen flex items-center justify-center bg-gray-50">
            <div className="w-80 bg-white p-6 border border-gray-300 rounded-lg">
                <h2 className="text-xl font-semibold mb-6 text-gray-800">Sign Up</h2>

                <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-4">
                <input
                    {...register("username", { required: true })}
                    placeholder="Username"
                    className="p-2 border border-gray-300 rounded focus:outline-none text-sm"
                />
                <input
                    {...register("firstname", { required: true })}
                    placeholder="First Name"
                    className="p-2 border border-gray-300 rounded focus:outline-none text-sm"
                />
                <input
                    {...register("lastname", { required: true })}
                    placeholder="Last Name"
                    className="p-2 border border-gray-300 rounded focus:outline-none text-sm"
                />
                <input
                    type="password"
                    {...register("password", { required: true })}
                    placeholder="Password"
                    className="p-2 border border-gray-300 rounded focus:outline-none text-sm"
                />
                <button className="bg-green-600 text-white py-2 rounded text-sm">
                    Sign Up
                </button>
                </form>

                <Link to="/login" className="block mt-4 text-center text-xs text-gray-500">
                    Back to Login
                </Link>
            </div>
        </div>
    )
}