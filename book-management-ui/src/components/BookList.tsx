import React, { useEffect, useState } from "react";
import { Button, Table, Container, Row, Col } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { BookDTO } from "../interfaces/interfaces";
import ConfirmDeleteModal from "./modals/ConfirmDeleteModal";
import LoadingSpinner from "./loader/LoadingSpinner";
import { deleteBook, fetchBooks } from "../services/bookService";
import PaginationControls from "./common/PaginationControls";

const BookList: React.FC = () => {
  const [books, setBooks] = useState<BookDTO[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [showDeleteModal, setShowDeleteModal] = useState<boolean>(false);
  const [bookToDelete, setBookToDelete] = useState<string | null>(null);
  const [deleting, setDeleting] = useState<boolean>(false);
  const [page, setPage] = useState<number>(1);
  const [pageSize] = useState<number>(5);
  const [totalPages, setTotalPages] = useState<number>(1);
  const navigate = useNavigate();

  useEffect(() => {
    setLoading(true);
    const loadBooks = async () => {
      try {
        const pagedResult = await fetchBooks(page, pageSize);
        setBooks(pagedResult.items);
        setTotalPages(pagedResult.totalPages);
        setLoading(false);
      } catch (error) {
        setLoading(false);
        console.error(error);
      }
    };

    loadBooks();
  }, [page, pageSize]);

  const handleDelete = (id: string) => {
    setBookToDelete(id);
    setShowDeleteModal(true);
  };

  const confirmDelete = async () => {
    if (bookToDelete) {
      setDeleting(true);
      try {
        await deleteBook(bookToDelete);
        setBooks(books.filter((book) => book.bookId !== bookToDelete));
        setShowDeleteModal(false);
        setDeleting(false);
      } catch (error) {
        setShowDeleteModal(false);
        setDeleting(false);
        console.error(error);
      }
    }
  };

  if (loading) {
    return <LoadingSpinner />;
  }

  return (
    <Container className="component-container">
      <Table striped bordered hover responsive>
        <thead>
          <tr>
            <th>Title</th>
            <th>Author</th>
            <th>Published Date</th>
            <th>Categories</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {books.map((book) => (
            <tr key={book.bookId}>
              <td>{book.title}</td>
              <td>{book.author}</td>
              <td>{new Date(book.publishedDate).toLocaleDateString()}</td>
              <td>{book.categoryNames.join(", ")}</td>
              <td>
                <Button
                  variant="primary"
                  onClick={() => navigate(`/books/${book.bookId}`)}
                >
                  View
                </Button>
                <Button
                  variant="secondary"
                  onClick={() => navigate(`/books/${book.bookId}/edit`)}
                >
                  Edit
                </Button>
                <Button
                  variant="danger"
                  onClick={() => handleDelete(book.bookId!)}
                >
                  Delete
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
      <PaginationControls
        page={page}
        totalPages={totalPages}
        onPageChange={setPage}
      />
      <ConfirmDeleteModal
        show={showDeleteModal}
        onHide={() => setShowDeleteModal(false)}
        onConfirm={confirmDelete}
      />

      {deleting && <LoadingSpinner />}

      <Row className="mt-3">
        <Col className="d-flex justify-content-start">
          <Button
            variant="outline-success"
            onClick={() => navigate("/books/new")}
            className="me-3"
            size="lg"
            data-testid="add-new-book-button"
          >
            Add New Book
          </Button>
          <Button
            variant="outline-dark"
            onClick={() => navigate("/")}
            size="lg"
          >
            Go to Home Page
          </Button>
        </Col>
      </Row>
    </Container>
  );
};

export default BookList;
