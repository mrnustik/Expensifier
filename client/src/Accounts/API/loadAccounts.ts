import { IAccountListItem } from "./IAccountListItem";

export async function loadAccounts() : Promise<ReadonlyArray<IAccountListItem>> {
    const response = await fetch('/api/accounts');
    return response.json();
}