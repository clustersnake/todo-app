export interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
}

export interface Task {
  id?: number;
  title: string;
  description: string;
  dueDate: string;
  tags: string[];
  priorityId: number; // 1: Low, 2: Medium, 3: High
  completed: boolean;
  userId: number;
}
