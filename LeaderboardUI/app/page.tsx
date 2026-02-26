import {PlayerForm} from "../components/playerForm";


export default function HomePage() {
  return (
    <div className="min-h-screen bg-gray-50 flex flex-col items-center justify-center p-6 text-center">
      <header className="max-w-2xl">
        <h1 className="text-5xl font-extrabold text-gray-900 mb-4 tracking-tight">
          Welcome to <span className="text-blue-600">Next.js 2026</span>
        </h1>
        <p className="text-lg text-gray-600 mb-8">
          This page was rendered on the server. No client-side JavaScript was 
          required to display this text, making it lightning fast.
        </p>
      </header>

      <PlayerForm/>

      <main className="grid grid-cols-1 md:grid-cols-2 gap-4 max-w-4xl w-full">
        <div className="p-6 bg-white rounded-xl shadow-sm border border-gray-200 hover:border-blue-500 transition-colors">
          <h2 className="font-bold text-xl mb-2 text-gray-800">Server Side →</h2>
          <p className="text-gray-600">Secure data fetching and SEO optimization out of the box.</p>
        </div>

        <div className="p-6 bg-white rounded-xl shadow-sm border border-gray-200 hover:border-blue-500 transition-colors">
          <h2 className="font-bold text-xl mb-2 text-gray-800">App Router →</h2>
          <p className="text-gray-600">Nested layouts and simplified routing architecture.</p>
        </div>
      </main>

      <footer className="mt-12 text-gray-400 text-sm">
        Built with the App Router structure.
      </footer>
    </div>
  );
}