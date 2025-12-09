import { client, type ResultOperation, type ResultOperationVoid } from "./client";
import { type State, type GetState } from "./state";

type Task = {
    id: number;
    title: string;
    description: string;
    stateId: number;
    dueDate: Date;
    createdAt: Date;
    updateAt: Date;
    state: State
};

export type GetTask = {
    Id: number;
    Title: string;
    Description: string;
    DueDate: Date;
    CreatedAt: Date;
    UpdateAt: Date;
    State: GetState
};

type CreateTask = {
    Title: string;
    Description: string;
    StateId: number;
    DueDate: Date;
};

type UpdateTask = {
    Id: number;
    Title: string;
    Description: string;
    StateId: number;
    DueDate: Date;
};

export type Pagination = {
    CurrentPage: number;
    PageSize: number;
    TotalCount: number;
    TotalPages: number;
    HasPreviousPage: boolean;
    HasNextPage: boolean;
};

type GetTasksResponse = {
    tasks: Array<GetTask>;
    metadata: Pagination;
};

export const getTasks = async (pageNumber : number) => {
    const result: ResultOperation<GetTasksResponse> = { data: {} as GetTasksResponse, isSuccess: true, message: '' };

    await client
        .get(`/Task/?pageNumber=${pageNumber}`)
        .then(res => {
            result.data!.tasks = res.data.tasks as Array<GetTask>;
            result.data!.metadata = res.data.metadata as Pagination;
        })
        .catch(err => {
            result.isSuccess = false;
            result.message = err;
        });
    return result;
}


export const createTask = async (task: CreateTask) => {
    const result: ResultOperationVoid = { isSuccess: true, message: '' };
    await client
        .post('/Task', task)
        .then(() => {
            result.message = 'Task created successfully';
        })
        .catch(err => {
            result.isSuccess = false;
            result.message = err.message;
        }
    );
    return result;
}

export const updateTask = async (task: UpdateTask) => {
    const result: ResultOperationVoid = { isSuccess: true, message: '' };
    await client
        .put('/Task', task)
        .then(() => {
            result.message = 'Task updated successfully';
        })
        .catch(err => {
            result.isSuccess = false;
            result.message = err.message;
        }
    );
    return result;
}

export const deleteTask = async (id: number) => {
    const result: ResultOperationVoid = { isSuccess: true, message: '' };
    await client
        .delete(`/Task/${id}`)
        .then(() => {
            result.message = 'Task deleted successfully';
        })
        .catch(err => {
            result.isSuccess = false;
            result.message = err.message;
        }
    );
}