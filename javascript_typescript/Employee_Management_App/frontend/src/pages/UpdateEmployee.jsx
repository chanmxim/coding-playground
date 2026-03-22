import { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useGetEmployeeDetails, useUpdateEmployee  } from "../hooks/employeeHooks";

import { Button } from "../components/ui/button"
import {
    Card,
    CardContent,
    CardFooter,
    CardHeader,
    CardTitle,
} from "../components/ui/card"
import { Input } from "../components/ui/input"
import { Label } from "../components/ui/label"

const initialEmployeeState = {
    first_name: "",
    last_name: "",
    email: "",
    position: "",
    salary: "",
    date_of_joining: "",
    department: "",
};

export function UpdateEmployee() {
    const { employeeId } = useParams();  
    
    const navigate = useNavigate();

    const [employee, setEmployee] = useState(initialEmployeeState); 
    const [image, setImage] = useState(null);

    const { data: response } = useGetEmployeeDetails(employeeId);
    const fetchedEmployee = response?.data || [];

    const { 
        mutate, 
        isSuccess: updateSuccess, 
        error: updateError 
    } = useUpdateEmployee();

    useEffect(() => {

        if (fetchedEmployee) {
            // Format as YYYY-MM-DD for the input[type=date]
            const dateOnly = fetchedEmployee.date_of_joining ? fetchedEmployee.date_of_joining.split('T')[0] : '';

            setEmployee({
                ...fetchedEmployee,
                date_of_joining: dateOnly,
            });
        }
    }, [fetchedEmployee]);
    
    useEffect(() => {
        if (updateSuccess) {
            setTimeout(() => {
                navigate("/dashboard");
            }, 5000);
        }
    }, [updateSuccess, navigate]);

    const handleFileChange = (e) => {
        setImage(e.target.files[0]);
    };

    const handleChange = (e) => {
        const { id, value } = e.target;
        setEmployee(prev => ({
            ...prev,
            [id]: value,
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        
        if (!employeeId) {
            console.error("Cannot update: Employee details are not loaded or ID is missing.");
            return;
        }

        const formData = new FormData();
        Object.keys(employee).forEach(key => {
            formData.append(key, employee[key]);
        });

        if (image) {
            formData.append("photo", image);
        }
        
        mutate({ id: employeeId, formData: formData });
    };
    
    const formTitle = `Edit Details for Employee ID: ${employeeId}`;

    return (
        <div className="flex items-center justify-center min-h-screen p-4 bg-gray-50">
            <Card className="w-full max-w-2xl shadow-xl">
                <CardHeader>
                    <CardTitle className="text-3xl font-bold text-center">{formTitle}</CardTitle>
                </CardHeader>

                {fetchedEmployee ? (
                    <form onSubmit={handleSubmit}>
                        <CardContent className="space-y-6">
                            {/* Status Messages */}
                            {updateError && (
                                <div className="p-3 bg-red-100 border border-red-400 text-red-700 rounded-lg">
                                    Update Error: {updateError.message || "Failed to update employee."}
                                </div>
                            )}
                            {updateSuccess && (
                                <div className="p-3 bg-green-100 border border-green-400 text-green-700 rounded-lg">
                                    Success! Employee updated. Redirecting to the dashboard in 5 seconds...
                                </div>
                            )}
                            
                            {/* Personal Details Group */}
                            <h2 className="text-xl font-semibold border-b pb-2">Personal Details</h2>
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                                {/* First Name */}
                                <div className="grid gap-2">
                                    <Label htmlFor="first_name">First Name</Label>
                                    <Input
                                        id="first_name"
                                        type="text"
                                        placeholder="Jane"
                                        required
                                        value={employee.first_name}
                                        onChange={handleChange}
                                    />
                                </div>
                                {/* Last Name */}
                                <div className="grid gap-2">
                                    <Label htmlFor="last_name">Last Name</Label>
                                    <Input
                                        id="last_name"
                                        type="text"
                                        placeholder="Doe"
                                        required
                                        value={employee.last_name}
                                        onChange={handleChange}
                                    />
                                </div>
                                {/* Email */}
                                <div className="grid gap-2 col-span-1 md:col-span-2">
                                    <Label htmlFor="email">Email</Label>
                                    <Input
                                        id="email"
                                        type="email"
                                        placeholder="jane.doe@company.com"
                                        required
                                        value={employee.email}
                                        onChange={handleChange}
                                    />
                                </div>
                            </div>

                            {/* Employment Details Group */}
                            <h2 className="text-xl font-semibold border-b pb-2 mt-8">Employment Details</h2>
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                                {/* Position */}
                                <div className="grid gap-2">
                                    <Label htmlFor="position">Position</Label>
                                    <Input
                                        id="position"
                                        type="text"
                                        placeholder="Software Engineer"
                                        required
                                        value={employee.position}
                                        onChange={handleChange}
                                    />
                                </div>
                                {/* Department */}
                                <div className="grid gap-2">
                                    <Label htmlFor="department">Department</Label>
                                    <Input
                                        id="department"
                                        type="text"
                                        placeholder="Technology"
                                        required
                                        value={employee.department}
                                        onChange={handleChange}
                                    />
                                </div>
                                {/* Salary */}
                                <div className="grid gap-2">
                                    <Label htmlFor="salary">Annual Salary ($)</Label>
                                    <Input
                                        id="salary"
                                        type="number"
                                        placeholder="75000"
                                        value={employee.salary}
                                        onChange={handleChange}
                                    />
                                </div>
                                {/* Date of Joining */}
                                <div className="grid gap-2">
                                    <Label htmlFor="date_of_joining">Date of Joining</Label>
                                    <Input
                                        id="date_of_joining"
                                        type="date"
                                        required
                                        value={employee.date_of_joining}
                                        onChange={handleChange}
                                    />
                                </div>
                                {/* Upload Photo */}
                                <div className="grid gap-2 col-span-1 md:col-span-2">
                                    <Label htmlFor="photo">Upload Photo</Label>
                                    <Input
                                        id="photo"
                                        type="file"
                                        accept="image/*"
                                        onChange={handleFileChange}
                                    />
                                </div>
                            </div>
                        </CardContent>
                        
                        {/* Buttons */}
                        <CardFooter className="flex justify-between gap-4 pt-6 border-t">
                            <Button 
                                variant="outline" 
                                type="button" 
                                onClick={() => navigate("/dashboard")}
                            >
                                Cancel
                            </Button>
                            <Button 
                                type="submit" 
                                className="px-8"
                            >
                                Update
                            </Button>
                        </CardFooter>
                    </form>
                ) : (
                    <div className="text-center py-20">Loading employee details...</div>
                )}
            </Card>
        </div>
    );
}