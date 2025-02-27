import * as Yup from "yup";
import { NavigateFunction } from "react-router-dom";

export const categoryOptions = [
  { value: "Technology", label: "Technology" },
  { value: "Travel", label: "Travel" },
  { value: "Western", label: "Western" },
  { value: "Philosophy", label: "Philosophy" },
  { value: "History", label: "History" },
  { value: "Fiction", label: "Fiction" },
  { value: "Psychology", label: "Psychology" },
  { value: "Romance", label: "Romance" },
  { value: "Non-Fiction", label: "Non-Fiction" },
  { value: "Biography", label: "Biography" },
];

export const validationSchema = Yup.object({
  title: Yup.string().required("Title is required"),
  author: Yup.string().required("Author is required"),
  publishedDate: Yup.date()
    .required("Published Date is required")
    .max(new Date(), "Published Date cannot be in the future"),
  categoryNames: Yup.array().min(1, "At least one category is required"),
});

export const handleModalClose = (setShowModal: (show: boolean) => void) => () =>
  setShowModal(false);

export const handleModalConfirm =
  (setShowModal: (show: boolean) => void, navigate: NavigateFunction) => () => {
    setShowModal(false);
    navigate(-1);
  };

export const handleGoBack =
  (
    isFormDirty: boolean,
    setShowModal: (show: boolean) => void,
    navigate: NavigateFunction
  ) =>
  () => {
    if (isFormDirty) {
      setShowModal(true);
    } else {
      navigate(-1);
    }
  };

export const formatDate = (dateString: string) => {
  const date = new Date(dateString);
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const day = String(date.getDate()).padStart(2, "0");
  return `${year}-${month}-${day}`;
};
