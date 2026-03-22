import { useNavigate } from "react-router-dom";
import { useGetEmployees, useSearchEmployees } from "../hooks/employeeHooks";
import { useState } from "react";

import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "../components/ui/table"

import { Button } from "../components/ui/button";
import { Input } from "../components/ui/input";
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuSeparator,
    DropdownMenuTrigger,
} from "../components/ui/dropdown-menu";
import { MoreHorizontal } from "lucide-react";

export function Dashboard() {
    const [department, setDepartment] = useState("");
    const [searchTerm, setSearchTerm] = useState("");

    const navigate = useNavigate();

    const { data: allResponse, error } = useGetEmployees();
    const employees = allResponse?.data || [];

    const { data: searchResponse } = useSearchEmployees(searchTerm);
    const employeesByDepartment = searchTerm ? (searchResponse?.data || []) : employees;

    const handleSearch = (e) => {
        e.preventDefault();
        setSearchTerm(department.trim());
    };

    const handleClearSearch = () => {
        setDepartment("");
        setSearchTerm("");
    };

    const handleAddEmployee = () => {
        navigate("/add")
    };

    const handleDetails = (employeeId) => {
        navigate(`/details/${employeeId}`);
    };

    const handleEdit = (employeeId) => {
        navigate(`/update/${employeeId}`);
    };

    const handleDelete = (employeeId) => {
        navigate(`/delete/${employeeId}`);
    };

    if (error) {
         return (
            <div className="flex items-center justify-center min-h-screen p-6 bg-gray-50">
                <div className="p-8 max-w-lg w-full bg-white rounded-xl shadow-xl border-l-4 border-red-500">
                    <h1 className="text-xl font-bold text-red-700 mb-2">Error Loading Data</h1>
                    <p className="text-gray-600">{error.message}</p>
                    <Button className="mt-6" onClick={() => window.location.reload()}>
                        Try Again
                    </Button>
                </div>
            </div>
        );
    }

    const dataToShow = searchTerm ? employeesByDepartment : employees;

    return (
        
        <div className="p-6">
            
            <div className="w-1/2 mx-auto">
                <h1 className="text-2xl font-bold mb-6 text-center">Employee Directory</h1>

                {/* Search input */}
                <form onSubmit={handleSearch} className="flex gap-2 mb-4">
                    <Input
                        type="text"
                        placeholder="Search by department"
                        value={department}
                        onChange={(e) => setDepartment(e.target.value)}
                    />
                    <Button 
                        type="submit" 
                        className="px-8"
                    >
                        Search
                    </Button>
                    {searchTerm && (
                        <Button variant="outline" onClick={handleClearSearch}>
                            Clear
                        </Button>
                    )}
                </form>

                {/* Add button */}
                <div className="flex justify-end items-center mb-4">
                    <Button onClick={handleAddEmployee}>
                        Add Employee
                    </Button>
                </div>
                
                {/* Table */}
                <Table>
                    <TableHeader>
                        <TableRow>
                            <TableHead>Full Name</TableHead>
                            <TableHead>Department</TableHead>
                            <TableHead>Email</TableHead>
                            <TableHead className="text-center w-[50px]">Actions</TableHead> 
                        </TableRow>
                    </TableHeader>
                    
                    <TableBody>
                        {dataToShow.length === 0 ? (
                            <TableRow>
                                <TableCell colSpan={5} className="text-center text-gray-500 py-6">
                                    No employee records found.
                                </TableCell>
                            </TableRow>
                        ) : (
                            dataToShow.map((employee) => (
                                <TableRow key={employee.id}>
                                    <TableCell className="font-medium">{`${employee.first_name} ${employee.last_name}`}</TableCell>
                                    <TableCell>{employee.department}</TableCell>
                                    <TableCell>{employee.email}</TableCell>
                                    
                                    <TableCell className="text-center">
                                        <DropdownMenu>
                                            <DropdownMenuTrigger asChild>
                                                <Button variant="ghost" className="h-8 w-8 p-0">
                                                    <span className="sr-only">Open menu</span>
                                                    <MoreHorizontal className="h-4 w-4" />
                                                </Button>
                                            </DropdownMenuTrigger>
                                            <DropdownMenuContent align="end">
                                                <DropdownMenuLabel>Actions</DropdownMenuLabel>
                                                <DropdownMenuItem onClick={() => handleDetails(employee.employee_id)}>
                                                    View Details
                                                </DropdownMenuItem>
                                                <DropdownMenuItem onClick={() => handleEdit(employee.employee_id)}>
                                                    Edit
                                                </DropdownMenuItem>
                                                <DropdownMenuSeparator />
                                                <DropdownMenuItem onClick={() => handleDelete(employee.employee_id)} className="text-red-600">
                                                    Delete
                                                </DropdownMenuItem>
                                            </DropdownMenuContent>
                                        </DropdownMenu>
                                    </TableCell>
                                </TableRow>
                            ))
                        )}
                    </TableBody>
                </Table>
            </div>
        </div>
    )
}