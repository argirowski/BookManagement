export interface BookDTO {
  bookId?: string;
  title: string;
  author: string;
  publishedDate: string;
  categoryNames: string[];
}

export interface ConfirmDeleteModalProps {
  show: boolean;
  onHide: () => void;
  onConfirm: () => void;
}

export interface ConfirmNavigationModalProps {
  show: boolean;
  onHide: () => void;
  onConfirm: () => void;
}
