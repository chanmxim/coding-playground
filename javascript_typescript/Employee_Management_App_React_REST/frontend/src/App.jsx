import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { Provider, useSelector } from 'react-redux';
import { store } from './redux/store';

import { Login } from './pages/auth/Login';
import { Signup } from './pages/auth/Signup';
import { Dashboard } from './pages/Dashboard';
import { AddEmployee } from './pages/AddEmployee';
import { UpdateEmployee } from './pages/UpdateEmployee';
import { DeleteEmployee } from './pages/DeleteEmployee';
import { EmployeeDetails } from './pages/EmployeeDetails';
import { Navbar } from './components/Navbar';

const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            refetchOnWindowFocus: false,
            retry: 1,
        }
    }
});

const ProtectedRoute = ({ children }) => {
  const { isAuthenticated } = useSelector((state) => state.auth);

  return isAuthenticated ? children : <Navigate to="/login" />;
};


function App() {

  return (
    <Provider store={store}>
      <QueryClientProvider client={queryClient}>
        
        <BrowserRouter>

          <Navbar />

          <Routes>
            <Route path="/login" element={<Login />}/>
            <Route path="/signup" element={<Signup />}/>


            <Route path="/dashboard" element={
              <ProtectedRoute>
                <Dashboard />
              </ProtectedRoute>
              }/>
            <Route path="/add" element={
              <ProtectedRoute>
                <AddEmployee />
              </ProtectedRoute>
              }/>
            <Route path="/update/:employeeId" element={
              <ProtectedRoute>
                <UpdateEmployee />
              </ProtectedRoute>
              }/>
            <Route path="/delete/:employeeId" element={
              <ProtectedRoute>
                <DeleteEmployee />
              </ProtectedRoute>
              }/>
            <Route path="/details/:employeeId" element={
              <ProtectedRoute>
                <EmployeeDetails />
              </ProtectedRoute>
              }/>
          </Routes>  
        </BrowserRouter>

      </QueryClientProvider>
    </Provider>
  )
}

export default App
