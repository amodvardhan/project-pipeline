import ProtectedRoute from '@/components/auth/ProtectedRoute';

export default function ProjectsLayout({
    children,
}: {
    children: React.ReactNode;
}) {
    return (
        <ProtectedRoute>
            <div className="container mx-auto p-6">
                {children}
            </div>
        </ProtectedRoute>
    );
}