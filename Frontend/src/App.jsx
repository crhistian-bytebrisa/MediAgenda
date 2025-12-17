import { Routes, Route, Navigate } from "react-router-dom";
import { ProtectedRoute } from "./routes/ProtectedRoute";
import { AdminHome } from "./pages/home/AdminHome";
import { DoctorHome } from "./pages/home/DoctorHome";
import { HomePage } from "./pages/home/HomePage";
import { LoginPage } from "./pages/LoginPage";
import { PatientHome } from "./pages/home/PatientHome";
import { RegisterPage } from "./pages/RegisterPage";
import { ProfilePage } from "./pages/ProfilePage";
import { useAuth } from "./context/AuthContext";
import { AnalysesCRUD } from "./pages/especialtypages/AnalysesPage";
import { ClinicsCRUD } from "./pages/especialtypages/ClinicsPage";
import { PermissionsCRUD } from "./pages/especialtypages/PermissionsPage";
import { MedicinesCRUD } from "./pages/especialtypages/MedicinesPage";
import { InsurancesCRUD } from "./pages/especialtypages/InsurancesPage";
import { PatientsList } from "./pages/especialtypages/PatientsPage";
import { PatientDetails } from "./pages/especialtypages/PatientDetailsPage";
import { ConsultationsList } from "./pages/especialtypages/ConsultationsPage";
import { ConsultationDetails } from "./pages/especialtypages/ConsultationDetailsPage";
import { UserConsultationsPage } from "./pages/home/UserConsultationsPage";
import { UserMedicinesPage } from "./pages/home/UserMedicinesPage";
import { UserDocumentsPage } from "./pages/home/UserDocumentsPage";
import DaysAvailableCRUD from "./pages/especialtypages/DaysAvailablePage";
import { SchedulePage } from "./pages/especialtypages/SchedulePage";
import { AppointmentPage } from "./pages/especialtypages/AppointmentPage";
import { TodayConsultationsPage } from "./pages/especialtypages/TodayConsultationsPage";
import { AttendConsultationPage } from "./pages/especialtypages/AttendConsultationPage";


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
