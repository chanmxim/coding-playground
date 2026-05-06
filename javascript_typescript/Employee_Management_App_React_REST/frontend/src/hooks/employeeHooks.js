import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { createEmployee, deleteEmployee, fetchEmployeeById, fetchEmployees, searchEmployeesByDepartment, updateEmployee } from "../services/employeeService";

// Constants to prevent typos in cache handling
export const EMPLOYEE_KEYS = {
    all: ["employees"],
    list: (filter) => [...EMPLOYEE_KEYS.all, "list", filter],
    details: (id) => [...EMPLOYEE_KEYS.all, "detail", id],
    department: (department_name) => [...EMPLOYEE_KEYS.all, "department", department_name]
};

// Queries
export const useGetEmployees = (searchTerm = "") => {
    return useQuery({
        queryKey: EMPLOYEE_KEYS.list(searchTerm),
        queryFn: () => fetchEmployees(searchTerm),
        keepPreviousData: true,
        staleTime: 5 * 60 * 1000 // 5 min
    });
};

export const useGetEmployeeDetails = (id) => {
    return useQuery({
        queryKey: EMPLOYEE_KEYS.details(id),
        queryFn: () => fetchEmployeeById(id),
        enabled: !!id
    })
}

export const useSearchEmployees = (department) => {
    return useQuery({
        queryKey: EMPLOYEE_KEYS.department(department),
        queryFn: () => searchEmployeesByDepartment(department),
        enabled: !!department,
        keepPreviousData: false,
    });
};

export const useAddEmployee = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (employeeData) => createEmployee(employeeData),
        onSuccess: () => {
            queryClient.invalidateQueries(EMPLOYEE_KEYS.all);
        }
    });
};

export const useUpdateEmployee = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: ({id, formData}) => updateEmployee(id, formData),
        onSuccess: (data, variables) => {
            queryClient.invalidateQueries(EMPLOYEE_KEYS.all);
            queryClient.invalidateQueries(EMPLOYEE_KEYS.details(variables.id));
        }
    });
};

export const useDeleteEmployee = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (id) => deleteEmployee(id),
        onSuccess: (data, variables) => {
            queryClient.invalidateQueries(EMPLOYEE_KEYS.all),
            queryClient.invalidateQueries(EMPLOYEE_KEYS.details(variables.id));
        }
    });
};