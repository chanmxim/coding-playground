import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { Button } from "../../components/ui/button"
import {
    Card,
    CardContent,
    CardFooter,
    CardHeader,
    CardTitle,
} from "../../components/ui/card"
import { Input } from "../../components/ui/input"
import { Label } from "../../components/ui/label"

import { useSignup } from "../../hooks/authHooks";

const initialUserState = {
    username: "",
    email: "",
    password: "",
};

export function Signup() {
    const [user, setUser] = useState(initialUserState);
    const { mutate, isSuccess, error } = useSignup();

    const navigate = useNavigate();

    const handleChange = (e) => {
        const { id, value } = e.target;
        setUser((prev) => ({
            ...prev,
            [id]: value,
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        mutate(user);
    };

    useEffect(() => {
        if (isSuccess) {
            setUser(initialUserState);
            const timer = setTimeout(() => navigate("/login"), 5000);
            return () => clearTimeout(timer);
        }
    }, [isSuccess, navigate]);

    return (
        <div className="flex items-center justify-center min-h-screen">
            <Card className="w-full max-w-sm">
                <CardHeader>
                    <CardTitle>Create an account</CardTitle>
                </CardHeader>

                <form onSubmit={handleSubmit}>
                    <CardContent>
                        {/* Status Messages */}
                        {error && (
                            <div className="p-3 bg-red-100 border border-red-400 text-red-700 rounded-lg">
                                Error: {error.message}
                            </div>
                        )}
                        {isSuccess && (
                            <div className="p-3 bg-green-100 border border-green-400 text-green-700 rounded-lg">
                                Account created! Redirecting to login in 5 seconds...
                            </div>
                        )}

                        <div className="flex flex-col gap-6">
                            <div className="grid gap-2">
                            <Label htmlFor="username">Username</Label>
                            <Input
                                id="username"
                                type="text"
                                placeholder="Enter your username"
                                required
                                value={user.username}
                                onChange={handleChange}
                            />
                            </div>
                            <div className="grid gap-2">
                            <Label htmlFor="email">Email</Label>
                            <Input
                                id="email"
                                type="email"
                                placeholder="example@domain.com"
                                required
                                value={user.email}
                                onChange={handleChange}
                            />
                            </div>
                            <div className="grid gap-2">
                                <div className="flex items-center">
                                    <Label htmlFor="password">Password</Label>
                                </div>
                                <Input 
                                    id="password" 
                                    type="password" 
                                    required 
                                    value={user.password}
                                    onChange={handleChange}
                                />
                            </div>
                        </div>
                    </CardContent>
                    
                    <CardFooter className="flex-col gap-2 ">
                        <Button type="submit" className="w-full">
                            Sign Up
                        </Button>
                        <Button 
                            variant="outline" 
                            className="w-full" 
                            onClick={() => navigate("/login")}
                        >
                            Proceed to Login
                        </Button>
                    </CardFooter>
                </form>
            </Card>
        </div>
    )
}
