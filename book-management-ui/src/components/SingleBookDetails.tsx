import React, { useEffect, useState } from "react";
import { Container, Row, Col, Button, Card } from "react-bootstrap";
import { useParams, useNavigate } from "react-router-dom";
import { BookDTO } from "../interfaces/interfaces";
import LoadingSpinner from "./loader/LoadingSpinner";
import { fetchBookById } from "../services/bookService";

const SingleBookDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [book, setBook] = useState<BookDTO | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const loadBook = async () => {
      try {
        const bookData = await fetchBookById(id!);
        setBook(bookData);
      } catch (error) {
        console.error("Error fetching book:", error);
      }
    };

    loadBook();
  }, [id]);

  if (!book) return <LoadingSpinner />;

  return (
    <Container className="component-container">
      <Row className="mt-3">
        <Col md={{ span: 8, offset: 2 }}>
          <Card>
            <Card.Header as="h2">Book Details</Card.Header>
            <Card.Body>
              <Card.Text>
                <span className="form-label">Title:</span>
                <span className="form-value"> {book.title}</span>
              </Card.Text>
              <Card.Text>
                <span className="form-label">Author:</span>
                <span className="form-value"> {book.author}</span>
              </Card.Text>
              <Card.Text>
                <span className="form-label">Published Date:</span>
                <span className="form-value">
                  {new Date(book.publishedDate).toLocaleDateString()}
                </span>
              </Card.Text>
              <Card.Text>
                <span className="form-label">Categories:</span>
                <span className="form-value">
                  {book.categoryNames.join(", ")}
                </span>
              </Card.Text>
              <div className="d-flex justify-content-start">
                <Button
                  variant="primary"
                  onClick={() => navigate(`/books/${book.bookId}/edit`)}
                  className="me-3"
                  size="lg"
                >
                  Edit Book
                </Button>
                <Button
                  variant="secondary"
                  onClick={() => navigate(-1)}
                  size="lg"
                >
                  Close
                </Button>
              </div>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default SingleBookDetails;
