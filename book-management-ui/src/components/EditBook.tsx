import React, { useState, useEffect } from "react";
import { Container, Row, Col, Button, Form } from "react-bootstrap";
import { useParams, useNavigate } from "react-router-dom";
import Select from "react-select";
import { Formik, Field, Form as FormikForm, ErrorMessage } from "formik";
import ConfirmNavigationModal from "./modals/ConfirmNavigationModal";
import { BookDTO } from "../interfaces/interfaces";
import {
  categoryOptions,
  validationSchema,
  handleModalClose,
  handleModalConfirm,
  handleGoBack,
  formatDate,
} from "../utils/formUtils";
import LoadingSpinner from "./loader/LoadingSpinner";
import { fetchBookById, updateBook } from "../services/bookService";

const EditBook: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [book, setBook] = useState<BookDTO | null>(null);
  const [showModal, setShowModal] = useState(false);
  const [isFormDirty, setIsFormDirty] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const loadBook = async () => {
      try {
        const bookData = await fetchBookById(id!);
        bookData.publishedDate = formatDate(bookData.publishedDate);
        setBook(bookData);
      } catch (error) {
        console.error("Error fetching book:", error);
      }
    };

    loadBook();
  }, [id]);

  const handleSubmit = async (values: BookDTO) => {
    try {
      const response = await updateBook(values);
      console.log("Edit book:", response);
      navigate("/books");
    } catch (error) {
      console.error("Error editing book:", error);
    }
  };

  if (!book) return <LoadingSpinner />;

  return (
    <Container className="component-container">
      <Row>
        <Col md={{ span: 8, offset: 2 }}>
          <h2>Edit Book</h2>
        </Col>
      </Row>
      <Formik
        initialValues={book}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
        enableReinitialize
      >
        {({ setFieldValue, handleChange, values }) => (
          <FormikForm>
            <Row className="mt-3">
              <Col md={{ span: 8, offset: 2 }}>
                <Form.Group controlId="formTitle">
                  <Form.Label>Title</Form.Label>
                  <Field
                    name="title"
                    type="text"
                    className="form-control"
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                      handleChange(e);
                      setIsFormDirty(true);
                    }}
                  />
                  <ErrorMessage
                    name="title"
                    component="div"
                    className="text-danger"
                  />
                </Form.Group>
              </Col>
            </Row>
            <Row className="mt-3">
              <Col md={{ span: 8, offset: 2 }}>
                <Form.Group controlId="formAuthor">
                  <Form.Label>Author</Form.Label>
                  <Field
                    name="author"
                    type="text"
                    className="form-control"
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                      handleChange(e);
                      setIsFormDirty(true);
                    }}
                  />
                  <ErrorMessage
                    name="author"
                    component="div"
                    className="text-danger"
                  />
                </Form.Group>
              </Col>
            </Row>
            <Row className="mt-3">
              <Col md={{ span: 8, offset: 2 }}>
                <Form.Group controlId="formPublishedDate">
                  <Form.Label>Published Date</Form.Label>
                  <Field
                    name="publishedDate"
                    type="date"
                    className="form-control"
                    onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                      handleChange(e);
                      setIsFormDirty(true);
                    }}
                  />
                  <ErrorMessage
                    name="publishedDate"
                    component="div"
                    className="text-danger"
                  />
                </Form.Group>
              </Col>
            </Row>
            <Row className="mt-3">
              <Col md={{ span: 8, offset: 2 }}>
                <Form.Group controlId="formCategories">
                  <Form.Label>Categories</Form.Label>
                  <Select
                    isMulti
                    name="categoryNames"
                    options={categoryOptions}
                    className="basic-multi-select"
                    classNamePrefix="select"
                    value={categoryOptions.filter((option) =>
                      values.categoryNames.includes(option.value)
                    )}
                    onChange={(selectedOptions) => {
                      setFieldValue(
                        "categoryNames",
                        selectedOptions
                          ? selectedOptions.map((option: any) => option.value)
                          : []
                      );
                      setIsFormDirty(true);
                    }}
                  />
                  <ErrorMessage
                    name="categoryNames"
                    component="div"
                    className="text-danger"
                  />
                </Form.Group>
              </Col>
            </Row>
            <Row className="mt-3">
              <Col
                md={{ span: 8, offset: 2 }}
                className="d-flex justify-content-start"
              >
                <Button
                  variant="outline-success"
                  type="submit"
                  className="me-3"
                  size="lg"
                >
                  Save Changes
                </Button>
                <Button
                  variant="outline-dark"
                  onClick={handleGoBack(isFormDirty, setShowModal, navigate)}
                  size="lg"
                >
                  Go Back
                </Button>
              </Col>
            </Row>
          </FormikForm>
        )}
      </Formik>

      <ConfirmNavigationModal
        show={showModal}
        onHide={handleModalClose(setShowModal)}
        onConfirm={handleModalConfirm(setShowModal, navigate)}
      />
    </Container>
  );
};

export default EditBook;
