import { useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useDeleteEmployee  } from "../hooks/employeeHooks";

import { Button } from "../components/ui/button"
import {
    Card,
    CardContent,
    CardFooter,
    CardHeader,
    CardTitle,
} from "../components/ui/card"

export function DeleteEmployee() {
    const { employeeId } = useParams();
    const navigate = useNavigate();

    const { 
        mutate: deleteMutate, 
        isSuccess: deleteSuccess, 
        error: deleteError 
    } = useDeleteEmployee();

    // Handle successful deletion and redirection
    useEffect(() => {
        if (deleteSuccess) {
            const timer = setTimeout(() => {
                navigate("/dashboard");
            }, 5000);
            
            return () => clearTimeout(timer);
        }
    }, [deleteSuccess, navigate]);

    // Handle the deletion action
    const handleDelete = () => {
        if (!employeeId) {
            console.error("Deletion prevented: Missing ID or already deleting.");
            return;
        }
        // Trigger the deletion mutation
        deleteMutate(employeeId);
    };

    if (deleteSuccess) {
        return (
            <div className="flex items-center justify-center min-h-screen p-4 bg-gray-50">
                <Card className="w-full max-w-lg text-center border-green-400 border-2">
                    <CardContent className="space-y-4">
                        <svg className="w-16 h-16 text-green-500 mx-auto" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
                        <CardTitle className="text-green-700">Deletion Successful</CardTitle>
                        <p className="text-gray-600">Employee ID **{employeeId}** has been permanently removed.</p>
                        <p className="text-gray-600">Redirecting to the dashboard in 5 seconds...</p>
                    </CardContent>
                </Card>
            </div>
        );
    }
    
    // Check if employeeId is missing (e.g., accessed /delete without parameter)
    if (!employeeId) {
        return (
            <div className="flex items-center justify-center min-h-screen p-4 bg-gray-50">
                <Card className="w-full max-w-lg text-center border-red-400 border-2">
                    <CardTitle className="text-red-700">Error: Missing ID</CardTitle>
                    <CardContent>
                        <p className="text-gray-600">No employee ID was provided in the URL to delete.</p>
                        <Button className="mt-6" onClick={() => navigate("/dashboard")}>
                            Go to Dashboard
                        </Button>
                    </CardContent>
                </Card>
            </div>
        );
    }

    // --- Main Confirmation UI ---
    return (
        <div className="flex items-center justify-center min-h-screen p-4 bg-gray-50">
            <Card className="w-full max-w-xl shadow-2xl border-l-8 border-red-600">
                <CardHeader>
                    <CardTitle className="text-3xl text-red-700 text-center">Confirm Permanent Deletion</CardTitle>
                </CardHeader>
                
                <CardContent className="space-y-4 text-center">
                    <p className="text-lg font-semibold text-gray-800">
                        You are about to permanently delete employee record:
                    </p>
                    <p className="text-4xl font-extrabold text-red-600 bg-red-50 p-3 rounded-lg border border-red-200 inline-block">
                        ID: {employeeId}
                    </p>
                    
                    {/* Deletion Error Message */}
                    {deleteError && (
                        <div className="p-3 bg-red-100 border border-red-400 text-red-700 rounded-lg text-left">
                            <strong>Deletion Failed:</strong> {deleteError.message}
                        </div>
                    )}

                </CardContent>
                
                <CardFooter className="justify-center pt-8">
                    <Button 
                        variant="outline" 
                        onClick={() => navigate("/dashboard")}
                    >
                        Cancel
                    </Button>
                    <Button 
                        variant="destructive" 
                        onClick={handleDelete}
                        className="min-w-[150px]"
                    >
                        Delete
                    </Button>
                </CardFooter>
            </Card>
        </div>
    );
}