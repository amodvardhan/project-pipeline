import EditProjectForm from '@/components/projects/EditProjectForm';

interface EditProjectPageProps {
    params: {
        id: string;
    };
}

export default function EditProjectPage({ params }: EditProjectPageProps) {
    return <EditProjectForm projectId={params.id} />;
}