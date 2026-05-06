import { gql } from 'apollo-angular';

export const LOGIN_USER = gql`
  query Login($usernameOrEmail: String!, $password: String!) {
    login(usernameOrEmail: $usernameOrEmail, password: $password) {
      token
      user {
        id
        username
        email
      }
    }
  }
`;

export const SIGNUP_USER = gql`
  mutation Signup($username: String!, $email: String!, $password: String!) {
    signup(username: $username, email: $email, password: $password) {
      token
      user {
        id
        username
        email
      }
    }
  }
`;

export const GET_ALL_EMPLOYEES = gql`
  query GetAllEmployees {
    getAllEmployees {
      id
      first_name
      last_name
      email
      designation
      department
    }
  }
`;

export const GET_EMPLOYEE_BY_ID = gql`
  query GetEmployeeById($id: ID!) {
    searchEmployeeByEid(id: $id) {
      id
      first_name
      last_name
      email
      gender
      designation
      salary
      date_of_joining
      department
      employee_photo
    }
  }
`;

export const SEARCH_EMPLOYEES = gql`
  query Search($designation: String, $department: String) {
    searchEmployeeByDesignationOrDepartment(designation: $designation, department: $department) {
      id
      first_name
      last_name
      email
      designation
      department
    }
  }
`;

export const ADD_NEW_EMPLOYEE = gql`
  mutation AddNewEmployee(
    $first_name: String!, 
    $last_name: String!, 
    $email: String!, 
    $gender: String!, 
    $designation: String!, 
    $salary: Float!, 
    $date_of_joining: String!, 
    $department: String!, 
    $employee_photo: String
  ) {
    addNewEmployee(
      first_name: $first_name, 
      last_name: $last_name, 
      email: $email, 
      gender: $gender, 
      designation: $designation, 
      salary: $salary, 
      date_of_joining: $date_of_joining, 
      department: $department, 
      employee_photo: $employee_photo
    ) {
      id
    }
  }
`;

export const UPDATE_EMPLOYEE = gql`
  mutation UpdateEmployee(
    $id: ID!,
    $first_name: String,
    $last_name: String,
    $email: String,
    $gender: String,
    $designation: String,
    $salary: Float,
    $department: String,
    $employee_photo: String
  ) {
    updateEmployeeByEid(
      id: $id,
      first_name: $first_name,
      last_name: $last_name,
      email: $email,
      gender: $gender,
      designation: $designation,
      salary: $salary,
      department: $department,
      employee_photo: $employee_photo
    ) {
      id
    }
  }
`;

export const DELETE_EMPLOYEE = gql`
  mutation DeleteEmployee($id: ID!) {
    deleteEmployeeByEid(id: $id)
  }
`;