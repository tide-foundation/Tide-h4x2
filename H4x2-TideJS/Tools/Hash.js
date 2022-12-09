
/**
 * @param {string} message 
 * @returns 
 */
export async function SHA256_Digest(message) {
  const encoder = new TextEncoder();
  const data = encoder.encode(message);
  const hash = await crypto.subtle.digest('SHA-256', data);
  return new Uint8Array(hash);
}