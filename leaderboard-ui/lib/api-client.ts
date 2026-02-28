export async function get<T>(url: string): Promise<T> {
    console.log("Fetching from URL:", url);
    const res = await fetch(url, {
        method: "GET",
    });
    console.log("API Response:", res);

    if (!res.ok) {
        throw new Error(`API error: ${res.status}`);
    }

    const data: T = await res.json();
    return data;
}