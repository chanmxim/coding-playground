import { Routes } from '@angular/router';
import { Login } from './components/login/login';
import { Signup } from './components/signup/signup';
import { EmployeeList } from './components/employee-list/employee-list';
import { authGuard } from './guards/auth-guard';
import { AddEmployee } from './components/add-employee/add-employee';
import { EditEmployee } from './components/edit-employee/edit-employee';
import { EmployeeDetails } from './components/employee-details/employee-details';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full'},
    { path: 'login', component: Login},
    { path: 'signup', component:  Signup},
    { path: 'employees', component:  EmployeeList, canActivate: [authGuard]},
    { path: 'add-employee', component:  AddEmployee, canActivate: [authGuard]},
    { path: 'edit-employee/:id', component: EditEmployee, canActivate: [authGuard] },
    { path: 'view-employee/:id', component: EmployeeDetails, canActivate: [authGuard] }
];
