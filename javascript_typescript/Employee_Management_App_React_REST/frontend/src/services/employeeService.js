import api from "../api/axiosClient";

export const fetchEmployees = async (query = "") => {
    const params = query ? { search: query } : {};
    const response = await api.get(`/emp/employees`, { params });

    return response.data;
}

export const fetchEmployeeById = async (id) => {
    const response = await api.get(`/emp/employees/${id}`);
    
    return response.data;
}

export const searchEmployeesByDepartment = async (department) => {
    const params = department ? { department } : {};
    const response = await api.get("/emp/employees/search", { params });
    return response.data;
};

export const createEmployee = async (employeeData) => {
    const response = await api.post(`/emp/employees`, employeeData)

    return response.data;
}

export const updateEmployee = async (id, employeeData) => {
    const response = await api.put(`/emp/employees/${id}`, employeeData)

    return response.data;
}

export const deleteEmployee = async (id) => {
    const response = await api.delete(`/emp/employees/${id}`)

    return response.data;
}