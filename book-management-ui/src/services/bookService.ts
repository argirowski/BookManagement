import axios from "axios";
import { BookDTO } from "../interfaces/interfaces";

export const fetchBooks = async () => {
  try {
    const response = await axios.get("https://localhost:7272/api/books");
    return response.data;
  } catch (error) {
    console.error("Error fetching books:", error);
    throw error;
  }
};

export const fetchBookById = async (id: string) => {
  try {
    const response = await axios.get(`https://localhost:7272/api/books/${id}`);
    return response.data;
  } catch (error) {
    console.error("Error fetching book:", error);
    throw error;
  }
};

export const deleteBook = async (id: string) => {
  try {
    await axios.delete(`https://localhost:7272/api/books/${id}`);
  } catch (error) {
    console.error("Error deleting book:", error);
    throw error;
  }
};

export const addBook = async (book: BookDTO) => {
  try {
    const response = await axios.post("https://localhost:7272/api/books", book);
    return response.data;
  } catch (error) {
    console.error("Error adding book:", error);
    throw error;
  }
};

export const updateBook = async (book: BookDTO) => {
  try {
    const response = await axios.put(
      `https://localhost:7272/api/books/${book.bookId}`,
      book
    );
    return response.data;
  } catch (error) {
    console.error("Error updating book:", error);
    throw error;
  }
};
