import { Routes, Route, Navigate } from "react-router-dom";
import { ProtectedRoute } from "./Routes/ProtectedRoute";
import { AdminHome } from "./Pages/home/AdminHome";
import { DoctorHome } from "./Pages/home/DoctorHome";
import { HomePage } from "./Pages/home/HomePage";
import { LoginPage } from "./Pages/LoginPage";
import { PatientHome } from "./Pages/home/PatientHome";
import { RegisterPage } from "./Pages/RegisterPage";
import { ProfilePage } from "./Pages/ProfilePage";
import { useAuth } from "./Context/AuthContext";
import { AnalysesCRUD } from "./Pages/Especialtypages/AnalysesPage";
import { ClinicsCRUD } from "./Pages/Especialtypages/ClinicsPage";
import { PermissionsCRUD } from "./Pages/Especialtypages/PermissionsPage";
import { MedicinesCRUD } from "./Pages/Especialtypages/MedicinesPage";
import { InsurancesCRUD } from "./Pages/Especialtypages/InsurancesPage";
import { PatientsList } from "./Pages/Especialtypages/PatientsPage";
import { PatientDetails } from "./Pages/Especialtypages/PatientDetailsPage";
import { ConsultationsList } from "./Pages/Especialtypages/ConsultationsPage";
import { ConsultationDetails } from "./Pages/Especialtypages/ConsultationDetailsPage";
import { UserConsultationsPage } from "./Pages/home/UserConsultationsPage";
import { UserMedicinesPage } from "./Pages/home/UserMedicinesPage";
import { UserDocumentsPage } from "./Pages/home/UserDocumentsPage";
import DaysAvailableCRUD from "./Pages/Especialtypages/DaysAvailablePage";
import { SchedulePage } from "./Pages/Especialtypages/SchedulePage";
import { AppointmentPage } from "./Pages/Especialtypages/AppointmentPage";
import { TodayConsultationsPage } from "./Pages/Especialtypages/TodayConsultationsPage";
import { AttendConsultationPage } from "./Pages/Especialtypages/AttendConsultationPage";


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
    <Routes>
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
    </Routes>
  );
}

export default App;
