import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Apollo } from 'apollo-angular';
import { DELETE_EMPLOYEE, GET_EMPLOYEE_BY_ID } from '../../graphql/operations';
import { Employee } from '../../models/Employee';

@Component({
  selector: 'app-employee-details',
  imports: [RouterLink, CurrencyPipe, DatePipe],
  templateUrl: './employee-details.html',
})
export class EmployeeDetails implements OnInit {
  private apollo = inject(Apollo);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  employee = signal<Employee | null>(null);
  isLoading = signal(true);

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.fetchEmployee(id);
    }
  }

  fetchEmployee(id: string) {
    this.apollo.query<any>({
      query: GET_EMPLOYEE_BY_ID,
      variables: { id },
      fetchPolicy: 'network-only'
    }).subscribe({
      next: (result) => {
        this.employee.set(result.data.searchEmployeeByEid);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error(err);
        this.isLoading.set(false);
      }
    });
  }

  handleDelete(id: string) {
    if (confirm('Are you sure you want to permanently delete this employee?')) {
      this.apollo.mutate({
        mutation: DELETE_EMPLOYEE,
        variables: { id }
      }).subscribe({
        next: () => {
          this.router.navigate(['/employees']);
        },
        error: (err) => alert("Delete failed: " + err.message)
      });
    }
  }
}
