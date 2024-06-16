export async function loadAccounts() : Promise<ReadonlyArray<IAccountListItem>> {
    const response = await fetch('/api/accounts');
    return response.json();
}

export interface IAccountListItem {
    id: string;
    userId: string;
    name: string;
}
