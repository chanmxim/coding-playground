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

import { useLogin } from "../../hooks/authHooks";

const initialState = {
    email: "",
    password: "",
};

export function Login() {
    const [credentials, setCredentials] = useState(initialState);
    const { mutate, isSuccess, error } = useLogin();

    const navigate = useNavigate();

    const handleChange = (e) => {
        const { id, value } = e.target;
        setCredentials((prev) => ({
            ...prev,
            [id]: value,
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        mutate(credentials);
    };

    useEffect(() => {
            if (isSuccess) {
                setCredentials(initialState);

                const timer = setTimeout(() => navigate("/dashboard"), 5000);

                return () => clearTimeout(timer);
            }
        }, [isSuccess, navigate]);

    return (
        <div className="flex items-center justify-center min-h-screen">
            <Card className="w-full max-w-sm">
                <CardHeader>
                    <CardTitle>Login to your account</CardTitle>
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
                                Login successful! Redirecting to dashboard in 5 seconds...
                            </div>
                        )}

                        <div className="flex flex-col gap-6">
                            <div className="grid gap-2">
                            <Label htmlFor="email">Email</Label>
                            <Input
                                id="email"
                                type="email"
                                placeholder="example@domain.com"
                                required
                                value={credentials.email}
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
                                    value={credentials.password}
                                    onChange={handleChange}
                                />
                            </div>
                        </div>
                    </CardContent>
                    
                    <CardFooter className="flex-col gap-2 ">
                        <Button type="submit" className="w-full">
                            Login
                        </Button>
                        <Button 
                            variant="outline" 
                            className="w-full" 
                            onClick={() => navigate("/signup")}
                        >
                            Proceed to Sign up
                        </Button>
                    </CardFooter>
                </form>
            </Card>
        </div>
    )
}
