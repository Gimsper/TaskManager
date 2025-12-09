import { client, type ResultOperation } from "./client";

export type State = {
    Id: number;
    Name: string;
    CreatedAt: Date;
    UpdatedAt: Date;
};

export type GetState = {
    Id: number;
    Name: string;
};

export const getStates = async () => {
    const result: ResultOperation<Array<GetState>> = { data: null, isSuccess: true, message: '' };
    await client.get('/State')
        .then(response => {
            result.data = response.data;
            console.log('Fetched states:', result.data);
        })
        .catch(error => {
            result.isSuccess = false;
            result.message = error.message;
        });
    return result;
}