import React, { useState } from "react";
import { Container, Row, Col, Button, Form } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
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
} from "../utils/formUtils";
import { addBook } from "../services/bookService";

const AddBook: React.FC = () => {
  const navigate = useNavigate();
  const [showModal, setShowModal] = useState(false);
  const [isFormDirty, setIsFormDirty] = useState(false);

  const initialValues: BookDTO = {
    title: "",
    author: "",
    publishedDate: "",
    categoryNames: [],
  };

  const handleSubmit = async (values: BookDTO) => {
    try {
      const response = await addBook(values);
      console.log("Book added:", response);
      navigate("/books");
    } catch (error) {
      console.error("Error adding book:", error);
    }
  };

  return (
    <Container className="component-container">
      <Row>
        <Col md={{ span: 8, offset: 2 }}>
          <h2>Add New Book</h2>
        </Col>
      </Row>
      <Formik
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
      >
        {({ setFieldValue, handleChange }) => (
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
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
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
                  variant="primary"
                  type="submit"
                  className="me-3"
                  size="lg"
                >
                  Add Book
                </Button>
                <Button
                  variant="secondary"
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

export default AddBook;
