import { useNavigate, useParams } from "react-router-dom";
import { useGetEmployeeDetails  } from "../hooks/employeeHooks";

import { Button } from "../components/ui/button"
import {
    Card,
    CardContent,
    CardFooter,
    CardHeader,
    CardTitle,
} from "../components/ui/card"

// Helper component for displaying a single field
const DetailField = ({ label, value }) => (
    <div className="flex justify-between items-center py-3 px-4 rounded-lg bg-gray-50 border border-gray-200 hover:bg-gray-100 transition duration-150">
        <span className="text-gray-600 font-medium tracking-wide">{label}</span>
        <span className="text-gray-900 font-bold">{value}</span>
    </div>
);

export function EmployeeDetails() {
    const { employeeId } = useParams();
    const navigate = useNavigate();

    const { data: response, error } = useGetEmployeeDetails(employeeId);
    const employee = response?.data || [];

    const fullName = employee ? `${employee.first_name} ${employee.last_name}` : 'Employee Record';

    if (error) {
        return (
            <div className="flex items-center justify-center min-h-screen p-4 bg-gray-50">
                <Card className="w-full max-w-2xl text-center border-l-4 border-red-500 bg-red-50">
                    <CardTitle className="text-red-700">Data Fetch Error</CardTitle>
                    <CardContent className="mt-4">
                        <p className="text-gray-600">{error.message || "Failed to load employee details."}</p>
                        <Button className="mt-6" onClick={() => navigate("/dashboard")}>
                            Return to Dashboard
                        </Button>
                    </CardContent>
                </Card>
            </div>
        );
    }
    
    // Fallback if data is null/undefined after loading (e.g., if ID was invalid but no explicit error was set)
    if (!employee) {
        return (
            <div className="flex items-center justify-center min-h-screen p-4 bg-gray-50">
                <Card className="w-full max-w-2xl text-center border-l-4 border-yellow-500 bg-yellow-50">
                    <CardTitle className="text-yellow-700">Employee Not Found</CardTitle>
                    <CardContent className="mt-4">
                        <p className="text-gray-600">The employee ID **{employeeId}** does not exist.</p>
                        <Button className="mt-6" onClick={() => navigate("/dashboard")}>
                            Return to Dashboard
                        </Button>
                    </CardContent>
                </Card>
            </div>
        );
    }


    // --- Main Employee Details UI ---
    return (
        <div className="flex items-center justify-center min-h-screen p-4 bg-gray-50">
            <Card className="w-full max-w-3xl">
                <CardHeader>
                    <CardTitle className="text-center text-indigo-700 border-b pb-2">
                        {fullName}
                    </CardTitle>
                    <p className="text-center text-sm text-gray-500">Employee ID: {employee.id}</p>
                </CardHeader>
                
                <CardContent className="space-y-4">
                    
                    {/* Profile Photo */}
                    {employee.photo && (
                        <div className="flex justify-center mb-6">
                            <img
                                src={`data:${employee.photoType};base64,${employee.photo}`}
                                alt="Employee"
                                className="w-40 h-40 object-cover rounded-full shadow-md border"
                            />
                        </div>
                    )}

                    <h3 className="text-xl font-semibold text-gray-700 mt-4 border-b pb-1">Contact & Personal</h3>
                    <DetailField label="Email" value={employee.email} />
                    
                    <h3 className="text-xl font-semibold text-gray-700 mt-8 border-b pb-1">Employment Details</h3>
                    <DetailField label="Position" value={employee.position} />
                    <DetailField label="Department" value={employee.department} />
                    <DetailField label="Salary" value={employee.salary} />
                    <DetailField label="Date of Joining" value={employee.date_of_joining?.split('T')[0]} />

                </CardContent>
                
                <CardFooter className="justify-between pt-8 border-t">
                    <Button 
                        variant="outline" 
                        onClick={() => navigate("/dashboard")}
                    >
                        &larr; Back to Dashboard
                    </Button>
                    <div className="flex gap-3">
                        <Button 
                            variant="destructive" 
                            onClick={() => navigate(`/delete/${employee.employee_id}`)}
                            className="bg-red-500 hover:bg-red-600"
                        >
                            Delete
                        </Button>
                        <Button 
                            onClick={() => navigate(`/update/${employee.employee_id}`)}
                        >
                            Edit Details
                        </Button>
                    </div>
                </CardFooter>
            </Card>
        </div>
    );
}