import { Component, inject, OnInit, signal } from '@angular/core';
import { Apollo } from 'apollo-angular';
import { Auth } from '../../services/auth/auth';
import { Employee } from '../../models/Employee';
import { DELETE_EMPLOYEE, GET_ALL_EMPLOYEES, SEARCH_EMPLOYEES } from '../../graphql/operations';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-employee-list',
  imports: [RouterLink, FormsModule],
  templateUrl: './employee-list.html',
})
export class EmployeeList implements OnInit{
  private apollo = inject(Apollo);
  private auth = inject(Auth);
  
  employees = signal<Employee[]>([]);
  isLoading = signal<boolean>(true);
  searchText = '';

  ngOnInit() {
    this.loadEmployees();
  }

  loadEmployees(){
    this.isLoading.set(true);

    this.apollo.query<any>({
      query: GET_ALL_EMPLOYEES,
      fetchPolicy: 'network-only'
    })
    .subscribe({
      next: (result) => {
        this.employees.set(result.data.getAllEmployees);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error(err);
        this.isLoading.set(false);
      }
    })
  };

  onSearch(){
    if (!this.searchText.trim()){
      this.loadEmployees();
      return;
    }

    this.isLoading.set(true);

    this.apollo.query<any>({
      query: SEARCH_EMPLOYEES,
      variables: {
        designation: this.searchText,
        department: this.searchText
      }
    })
    .subscribe({
      next: (result) => {
        this.employees.set(
          result.data.searchEmployeeByDesignationOrDepartment
        );
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error(err);
        this.isLoading.set(false);
      }
    });
  }

  deleteEmployee(id: string){
    if (confirm('Are you sure you want to delete this record?')){
      this.apollo.mutate({
        mutation: DELETE_EMPLOYEE,
        variables: {id}
      }).subscribe({
        next: () => {
          this.employees.update(prev => prev.filter(e => e.id !== id));
        },
        error: (err) => {
          console.error(err);
          this.isLoading.set(false);
      }
      })
    }
  }

  logout() {
    this.auth.logout();
  }

}
