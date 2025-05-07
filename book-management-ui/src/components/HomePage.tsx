import React from "react";
import { Container, Row, Col } from "react-bootstrap";
import { Link } from "react-router-dom";

const HomePage: React.FC = () => {
  return (
    <Container className="d-flex flex-column align-items-center mt-5">
      <Row className="mt-2">
        <Col>
          <h1 className="text-center" data-testid="welcome-message">
            Welcome to the Book Management App
          </h1>
        </Col>
      </Row>
      <Row className="mt-2">
        <Col>
          <h2 className="text-center">
            If you want to add a new book,{" "}
            <Link to="/books/new" data-testid="add-book-link">
              click here
            </Link>
          </h2>
        </Col>
      </Row>
      <Row className="mt-2">
        <Col>
          <h2 className="text-center">
            If you want to check out our current book selection,
            <Link to="/books" data-testid="view-books-link">
              {" "}
              click here
            </Link>
          </h2>
        </Col>
      </Row>
    </Container>
  );
};

export default HomePage;
