import { routes, Route, Navigate } from "react-router-dom";
import { ProtectedRoute } from "./routes/ProtectedRoute";
import { AdminHome } from "./pages/Home/AdminHome";
import { DoctorHome } from "./pages/Home/DoctorHome";
import { HomePage } from "./pages/Home/HomePage";
import { LoginPage } from "./pages/LoginPage";
import { PatientHome } from "./pages/Home/PatientHome";
import { RegisterPage } from "./pages/RegisterPage";
import { ProfilePage } from "./pages/ProfilePage";
import { useAuth } from "./Context/Autcontext";
import { AnalysesCRUD } from "./pages/Especialtypages/AnalysesPage";
import { ClinicsCRUD } from "./pages/Especialtypages/ClinicsPage";
import { PermissionsCRUD } from "./pages/Especialtypages/PermissionsPage";
import { MedicinesCRUD } from "./pages/Especialtypages/MedicinesPage";
import { InsurancesCRUD } from "./pages/Especialtypages/InsurancesPage";
import { PatientsList } from "./pages/Especialtypages/PatientsPage";
import { PatientDetails } from "./pages/Especialtypages/PatientDetailsPage";
import { ConsultationsList } from "./pages/Especialtypages/ConsultationsPage";
import { ConsultationDetails } from "./pages/Especialtypages/ConsultationDetailsPage";
import { UserConsultationsPage } from "./pages/Home/UserConsultationsPage";
import { UserMedicinesPage } from "./pages/Home/UserMedicinesPage";
import { UserDocumentsPage } from "./pages/Home/UserDocumentsPage";
import DaysAvailableCRUD from "./pages/Especialtypages/DaysAvailablePage";
import { SchedulePage } from "./pages/Especialtypages/SchedulePage";
import { AppointmentPage } from "./pages/Especialtypages/AppointmentPage";
import { TodayConsultationsPage } from "./pages/Especialtypages/TodayConsultationsPage";
import { AttendConsultationPage } from "./pages/Especialtypages/AttendConsultationPage";


function App() {
  const {roles} = useAuth();
  var home = ""
    if (roles.includes("Admin")) {
    home = "/admin";
    } else if (roles.includes("Doctor")) {
      home = "/doctor";
    } else if (roles.includes("User")) {
      home = "/user";
    } else {
      // Si no tiene roles (no autenticado), lo mandamos a login/home
      home = "/";
    }

  return (
    <routes>
      <Route path="/admin" element={
        <ProtectedRoute allowedRoles={["Admin"]}>
          <AdminHome />
        </ProtectedRoute>
      } />

      <Route path="/doctor" element={
        <ProtectedRoute allowedRoles={["Doctor"]}>
          <DoctorHome />
        </ProtectedRoute>
      } />

      <Route path="/user" element={
        <ProtectedRoute allowedRoles={["User"]}>
          <PatientHome />
        </ProtectedRoute>
      } />

      <Route path="/profile" element={
        <ProtectedRoute allowedRoles={["User","Doctor","Admin"]}>
          <ProfilePage />
        </ProtectedRoute>
      } />

      <Route path="/analyses" element={
        <ProtectedRoute allowedRoles={["Doctor","Admin"]}>
          <AnalysesCRUD />
        </ProtectedRoute>
      } />

      <Route path="/clinics" element={
        <ProtectedRoute allowedRoles={["Doctor","Admin"]}>
          <ClinicsCRUD />
        </ProtectedRoute>
      } />

      <Route path="/permissions" element={
        <ProtectedRoute allowedRoles={["Doctor","Admin"]}>
          <PermissionsCRUD />
        </ProtectedRoute>
      } />

      <Route path="/medicines" element={
        <ProtectedRoute allowedRoles={["Doctor","Admin"]}>
          <MedicinesCRUD />
        </ProtectedRoute>
      } />

      <Route path="/patients" element={
        <ProtectedRoute allowedRoles={["Doctor","Admin"]}>
          <PatientsList />
        </ProtectedRoute>
      } />

      <Route path="/patients/:id" element={
        <ProtectedRoute allowedRoles={["Doctor","Admin","User"]}>
          <PatientDetails />
        </ProtectedRoute>
      } />

      <Route path="/insurances" element={
        <ProtectedRoute allowedRoles={["Doctor","Admin"]}>
          <InsurancesCRUD />
        </ProtectedRoute>
      } />

      <Route path="/consultations" element={
        <ProtectedRoute allowedRoles={["Doctor","Admin"]}>
          <ConsultationsList />
        </ProtectedRoute>
      } />

      <Route path="/consultations/:id" element={
        <ProtectedRoute allowedRoles={["Doctor","Admin","User"]}>
          <ConsultationDetails />
        </ProtectedRoute>
      } />

      <Route path="/daysavailable" element={
        <ProtectedRoute allowedRoles={["Doctor","Admin"]}>
          <DaysAvailableCRUD />
        </ProtectedRoute>
      } />

      <Route path="/todayconsultations" element={
        <ProtectedRoute allowedRoles={["Doctor","Admin"]}>
          <TodayConsultationsPage />
        </ProtectedRoute>
      } />

      <Route path="/consultations/:id/attend" element={
        <ProtectedRoute allowedRoles={["Doctor", "Admin"]}>
          <AttendConsultationPage />
        </ProtectedRoute>
      } />

      <Route path="/user/schedules" element={
        <ProtectedRoute allowedRoles={["User"]}>
          <SchedulePage />
        </ProtectedRoute>
      } />

      <Route path="/user/appointments" element={
        <ProtectedRoute allowedRoles={["User"]}>
          <AppointmentPage />
        </ProtectedRoute>
      } />

      

      <Route path="/user/consultations" element={
        <ProtectedRoute allowedRoles={["User"]}>
          <UserConsultationsPage />
        </ProtectedRoute>
      } />

      <Route path="/user/medications" element={
        <ProtectedRoute allowedRoles={["User"]}>
          <UserMedicinesPage />
        </ProtectedRoute>
      } />

      <Route path="/user/documents" element={
        <ProtectedRoute allowedRoles={["User"]}>
          <UserDocumentsPage />
        </ProtectedRoute>
      } />

      <Route path="/" element={<HomePage />} />

      <Route path="/home" element={<HomePage />} />

      <Route path="/login" element={<LoginPage />} />

      <Route path="/register" element={<RegisterPage/>}/>

      <Route path="/*" element={<Navigate to={home} />} />
    </routes>
  );
}

export default App;
